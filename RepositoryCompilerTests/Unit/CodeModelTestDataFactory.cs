using System.Collections.Generic;

namespace RepositoryCompilerTests.Unit
{
    public class CodeModelTestDataFactory
    {
        public IEnumerable<string> GetDoctorClassText()
        {
            return new[]
            {
                @"
            using System.Collections.Generic;
            namespace DoctorApp.Model.Data
            {
                public class Doctor
                {
                    public string Name { get; set; }
                    public string Email { get; set; }
                    public List<DateRange> HolidayDates { get; set; }

                    public Doctor(string name, string email)
                    {
                        Name = name;
                        Email = email;
                        HolidayDates = new List<DateRange>();
                    }

                    internal bool IsAvailable(DateRange timeSpan)
                    {
                        foreach (DateRange holiday in HolidayDates)
                        {
                            if (holiday.OverlapsWith(timeSpan)) return false;
                        }
                        return true;
                    }
                }
            }"
            };
        }

        public IEnumerable<string> GetGitAdapterClassText()
        {
            return new[]
            {
                @"
                using LibGit2Sharp;
                using RepositoryCompiler.CodeModel.CaDETModel;
                using System;
                using System.Collections.Generic;
                using System.IO;
                using System.Linq;

                namespace RepositoryCompiler.RepositoryAdapters
                {
                    public class GitRepositoryAdapter : ICodeRepositoryAdapter
                    {
                        private readonly string _gitSourcePath;
                        private readonly string _gitDestinationPath;
                        private readonly string _mainBranchName;
                        private readonly string _uname;
                        private readonly string _pass;
                        public GitRepositoryAdapter(Dictionary<string, string> settings)
                        {
                            _gitSourcePath = settings[""CodeRepository:GitSourcePath""];
                            _gitDestinationPath = settings[""CodeRepository:GitDestinationPath""];
                            _mainBranchName = settings[""CodeRepository:MainBranchName""];
                            _uname = settings.ContainsKey(""CodeRepository:Username"")
                                ? settings[""CodeRepository:Username""]
                                : ""TODO"";
                            _pass = settings.ContainsKey(""CodeRepository:Password"")
                                ? settings[""CodeRepository:Password""]
                                : ""TODO"";
                        }

                        public void CloneRepository()
                        {
                            //Requires refactoring along with the rest of the configuration.
                            //TODO: Rework once RepositoryRepository is established.
                            var co = new CloneOptions
                            {
                                CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials { Username = _uname, Password = _pass }
                            };
                            Repository.Clone(_gitSourcePath, _gitDestinationPath, co);
                        }

                        public bool CheckForNewCommits()
                        {
                            CheckoutMasterBranch();
                            CommitId localCommit = GetActiveCommit();
                            PullChanges();
                            bool changesOccurred = !localCommit.Equals(GetActiveCommit());
                            return changesOccurred;
                        }

                        private void PullChanges()
                        {
                            //The current code only works with public repositories.
                            //The nonsensical options below should be refactored and extracted into a function
                            //That will be called by this function as well as CloneRepository.
                            PullOptions options = new PullOptions
                            {
                                FetchOptions = new FetchOptions
                                {
                                    CredentialsProvider = (url, usernameFromUrl, types) =>
                                        new UsernamePasswordCredentials() { Username = _uname, Password = _pass }
                                }
                            };
                            var signature = new Signature(new Identity(_uname, _uname), DateTime.Now);

                            Commands.Pull(GetRepository(), signature, options);
                        }

                        public CommitId GetActiveCommit()
                        {
                            return GetCommits(1).First();
                        }
                        
                        public IEnumerable<CommitId> GetCommits(int numOfPreviousCommits)
                        {
                            return GetRepository().Commits.Take(numOfPreviousCommits).Select(commit => new CommitId(commit.Sha));
                        }

                        public IEnumerable<CaDETProject> ParseProjectCode(CommitId commit)
                        {
                            CheckoutCommit(commit);
                            return ParseDocuments();
                        }

                        private IEnumerable<CaDETProject> ParseDocuments()
                        {
                            //specific to C# - should extract to C# file identifier when appropriate
                            string[] allFiles = Directory.GetFiles(_gitDestinationPath, ""*.cs"", SearchOption.AllDirectories);
                            var retVal = List<CaDETProject>();
                            foreach(var file in allFiles) {
                                if(file != null && file != """")
                                {
                                    retVal.Add(new CaDETProject(s, File.ReadAllText(s), LanguageEnum.CSharp));
                                }
                            }
                            return retVal;
                        }

                        private void CheckoutMasterBranch()
                        {
                            CheckoutCommit(null);
                        }

                        private void CheckoutCommit(CommitId commit)
                        {
                            if (commit != null) Commands.Checkout(GetRepository(), commit.Hash);
                            else Commands.Checkout(GetRepository(), _mainBranchName);
                        }

                        private Repository GetRepository()
                        {
                            return new Repository(_gitDestinationPath);
                        }
                    }
                }

            "
            };
        }

        public IEnumerable<string> GetMultipleClassTexts()
        {
            return new[]
            {
                @"
            using System.Collections.Generic;
            namespace DoctorApp.Model.Data.DateR
            {
                public class DateRange
                {
                    public DateTime From { get; set; }
                    public DateTime To { get; set; }

                    public DateRange(DateTime from, DateTime to)
                    {
                        From = from;
                        To = to;
                        if(To.Equals(From)) return;
                    }
                    public bool OverlapsWith(DateRange timeSpan)
                    {
                        return !(From > timeSpan.To || To < timeSpan.From);
                    }
                }
            }", 
                @"
            using System.Collections.Generic;
            using DoctorApp.Model.Data.DateR;
            namespace DoctorApp.Model.Data
            {
                public class Doctor
                {
                    public string Test;
                    public string Name { get; set; }
                    public string Email { get; set; }
                    public List<DateRange> HolidayDates { get; set; }

                    public Doctor(string name, string email)
                    {
                        Name = name;
                        Email = email;
                        HolidayDates = new List<DateRange>();
                    }
                }
            }",
                @"
            using System.Collections.Generic;
            using DoctorApp.Model.Data.DateR;
            using DoctorApp.Model.Data;
            namespace DoctorApp.Model
            {
                public class DoctorService
                {
                    private List<Doctor> _doctors;
                    public Doctor FindAvailableDoctor(DateRange timeSpan)
                    {
                        foreach (Doctor d in _doctors)
                        {
                            foreach(DateRange holiday in d.HolidayDates)
                            {
                                d.Test = null;
                                if (!holiday.OverlapsWith(timeSpan)) return d;
                                LogChecked();
                            }
                        }
                        return null;
                    }
                    private void LogChecked()
                    {
                        return;
                    }
                }
            }"
            };
        }
    }
}