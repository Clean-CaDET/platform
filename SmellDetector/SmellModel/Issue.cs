using System;

namespace SmellDetector.SmellModel
{
    public class Issue
    {
        public SmellType IssueType { get; }

        public string CodeSnippetId { get; }

        public Issue(SmellType issueType, string codeSnippetId)
        {
            IssueType = issueType;
            CodeSnippetId = codeSnippetId;
        }

        public override bool Equals(object? obj)
        {
            return obj is Issue other &&
                   other.CodeSnippetId == CodeSnippetId &&
                   other.IssueType == IssueType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IssueType, CodeSnippetId);
        }
    }
}
