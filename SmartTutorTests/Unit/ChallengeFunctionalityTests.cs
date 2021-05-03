using System;
using System.Diagnostics;
using Xunit;

namespace SmartTutor.Tests.Unit
{
    public class ChallengeFunctionalityTests
    {
        [Fact]
        public void RunUnitTests()
        {
            
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                var procStartInfo = new ProcessStartInfo("dotnet", "test \"../../../../SmellDetectorTests\"")
                {
                    RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true
                };

                var proc = new Process {StartInfo = procStartInfo};
                proc.Start();
                
                string result = proc.StandardOutput.ReadToEnd();
            }
            catch (Exception objException)
            {
                // Log the exception
            }
        }
    }
}
