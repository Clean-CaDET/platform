using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesivePartsOutput
    {
        private string AccessesToRemove { get; }
        private List<string> TextsOfParts { get; }

        public CohesivePartsOutput(string accessesToRemove, List<string> textsOfTextsOfParts)
        {
            AccessesToRemove = accessesToRemove;
            TextsOfParts = textsOfTextsOfParts;
        }


        public override bool Equals(object obj)
        {
            if (obj is not CohesivePartsOutput cohesivePartsOutput) return false;

            if (cohesivePartsOutput.AccessesToRemove != AccessesToRemove) return false;

            return cohesivePartsOutput.TextsOfParts.All(textOfPart =>
                cohesivePartsOutput.TextsOfParts.Contains(textOfPart));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccessesToRemove, TextsOfParts);
        }
    }
}