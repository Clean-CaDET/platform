using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    public class MetricRangeRule
    {
        [Key] public int Id { get; set; }
        public string MetricName { get; internal set; }
        public double FromValue { get; internal set; }
        public double ToValue { get; internal set; }

        public bool MetricMeetsRequirements(Dictionary<CaDETMetric, double> metrics)
        {
            var metricValue = metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            return (FromValue <= metricValue && metricValue <= ToValue);
        }

        public ChallengeHint GetHintForIncompleteClass()
        {
            string content;

            switch (MetricName)
            {
                #region ClassMetrics
                case "CLOC": // lines of code of a class
                    content = "Add or remove lines of the code in the class.";
                    break;
                case "LCOM": // lack of cohesion of methods
                    content = "Increase or decrease cohesion of methods in the class.";
                    break;
                case "NMD": // number of methods declared
                    content = "Increase or decrease number of methods declared in the class.";
                    break;
                case "NAD": // number of attributes defined
                    content = "Increase or decrease number of attributes defined in the class.";
                    break;
                case "NMD_NAD": // number of methods and attributes
                    content = "Increase or decrease number of methods and attributes in the class.";
                    break;
                case "WMC": // weighted methods per class
                    content = "Increase or decrease weighted methods for the class.";
                    break;
                case "ATFD": // access to foreign data
                    content = "Increase or decrease access to foreign data in the class.";
                    break;
                case "TCC": // tight class cohesion
                    content = "Increase or decrease tight class cohesion of the class.";
                    break;
                case "CNOR": // total number of return statements from all class members
                    content = "Increase or decrease total number of return statements from all class members in the class.";
                    break;
                case "CNOL": // total number of loops from all class members
                    content = "Increase or decrease total number of loops from all class members in the class.";
                    break;
                case "CNOC": // total number of comparison operators from all class members
                    content = "Increase or decrease total number of comparison operators from all class members in the class.";
                    break;
                case "CNOA": // total number of assignments from all class members
                    content = "Increase or decrease total number of assignments from all class members in the class.";
                    break;
                case "NOPM": // number of private methods
                    content = "Increase or decrease number of private methods in the class.";
                    break;
                case "NOPF": // number of protected fields
                    content = "Increase or decrease number of protected fields in the class.";
                    break;
                case "CMNB": // max nested blocks from all class members
                    content = "Increase or decrease max nested blocks from all class members in the class.";
                    break;
                case "RFC": // response for a class, counts the number of unique method invocations
                    content = "Increase or decrease number of unique method invocations in the class.";
                    break;
                #endregion
                default:
                    content = MetricName + " is the issue.";
                    break;
            }

            return new ChallengeHint
            {
                Content = content,
                LearningObjectSummaryId = 337
            };
        }

        public ChallengeHint GetHintForIncompleteMethod()
        {
            string content;

            switch (this.MetricName)
            {
                #region MethodMetrics
                case "CYCLO": // cyclomatic complexity
                    content = "Increase or decrease cyclomatic complexity in a method.";
                    break;
                case "MLOC": // lines of code in a method
                    content = "Increase or decrease lines of code in a method.";
                    break;
                case "MELOC": // effective lines of code of a method, excluding comments, blank lines, function header and function braces
                    content = "Increase or decrease effective lines of code of a method (excluding comments, blank lines, function header and function braces).";
                    break;
                case "NOP": // number of parameters
                    content = "Increase or decrease number of parameters in a method.";
                    break;
                case "NOLV": // number of local variables
                    content = "Increase or decrease number of local variables in a method.";
                    break;
                case "NOTC": // number of try catch blocks
                    content = "Increase or decrease number of try catch blocks in a method.";
                    break;
                case "MNOL": // number of loops in method
                    content = "Increase or decrease number of loops in a method.";
                    break;
                case "MNOR": // number of return statements in method
                    content = "Increase or decrease number of return statements in a method.";
                    break;
                case "MNOC": // number of comparison operators in method
                    content = "Increase or decrease number of comparison operators in a method.";
                    break;
                case "MNOA": // number of assignments in method
                    content = "Increase or decrease number of assignments in a method.";
                    break;
                case "NONL": // number of numeric literals 
                    content = "Increase or decrease number of numeric literals in a method.";
                    break;
                case "NOSL": // number of string literals
                    content = "Increase or decrease number of string literals in a method.";
                    break;
                case "NOMO": // number of math operations
                    content = "Increase or decrease number of math operations in a method.";
                    break;
                case "NOPE": // number of parenthesized expressions
                    content = "Increase or decrease number of parenthesized expressions in a method.";
                    break;
                case "NOLE": // number of lambda expressions
                    content = "Increase or decrease number of lambda expressions in a method.";
                    break;
                case "MMNB": // max nested blocks in the method
                    content = "Increase or decrease max nested blocks in the method.";
                    break;
                case "NOUW": // number of unique words
                    content = "Increase or decrease number of unique words in a method.";
                    break;
                #endregion
                default:
                    content = MetricName + " is the issue.";
                    break;
            }

            return new ChallengeHint
            {
                Content = content,
                LearningObjectSummaryId = 337
            };
        }

        public ChallengeHint GetApplicableHintForIncompleteClass(CaDETClass caDETClass)
        {
            var metricValue = caDETClass.Metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            string content;

            switch (MetricName)
            {
                #region ClassMetrics
                case "CLOC": // lines of code of a class
                    if (metricValue < FromValue)
                        content = "Add more lines of the code in the class " + caDETClass.Name + ".";
                    else
                        content = "Remove some lines of the code in the class " + caDETClass.Name + ".";
                    break;
                case "LCOM": // lack of cohesion of methods
                    if (metricValue < FromValue)
                        content = "Increase cohesion of methods in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease cohesion of methods in the class " + caDETClass.Name + ".";
                    break;
                case "NMD": // number of methods declared
                    if (metricValue < FromValue)
                        content = "Increase number of methods declared in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of methods declared in the class " + caDETClass.Name + ".";
                    break;
                case "NAD": // number of attributes defined
                    if (metricValue < FromValue)
                        content = "Increase number of attributes defined in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of attributes defined in the class " + caDETClass.Name + ".";
                    break;
                case "NMD_NAD": // number of methods and attributes
                    if (metricValue < FromValue)
                        content = "Increase number of methods and attributes in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of methods and attributes in the class " + caDETClass.Name + ".";
                    break;
                case "WMC": // weighted methods per class
                    if (metricValue < FromValue)
                        content = "Increase weighted methods for the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease weighted methods for the class " + caDETClass.Name + ".";
                    break;
                case "ATFD": // access to foreign data
                    if (metricValue < FromValue)
                        content = "Increase access to foreign data in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease access to foreign data in the class " + caDETClass.Name + ".";
                    break;
                case "TCC": // tight class cohesion
                    if (metricValue < FromValue)
                        content = "Increase tight class cohesion of the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease tight class cohesion of the class " + caDETClass.Name + ".";
                    break;
                case "CNOR": // total number of return statements from all class members
                    if (metricValue < FromValue)
                        content = "Increase total number of return statements from all class members in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease total number of return statements from all class members in the class " + caDETClass.Name + ".";
                    break;
                case "CNOL": // total number of loops from all class members
                    if (metricValue < FromValue)
                        content = "Increase total number of loops from all class members in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease total number of loops from all class members in the class " + caDETClass.Name + ".";
                    break;
                case "CNOC": // total number of comparison operators from all class members
                    if (metricValue < FromValue)
                        content = "Increase total number of comparison operators from all class members in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease total number of comparison operators from all class members in the class " + caDETClass.Name + ".";
                    break;
                case "CNOA": // total number of assignments from all class members
                    if (metricValue < FromValue)
                        content = "Increase total number of assignments from all class members in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease total number of assignments from all class members in the class " + caDETClass.Name + ".";
                    break;
                case "NOPM": // number of private methods
                    if (metricValue < FromValue)
                        content = "Increase number of private methods in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of private methods in the class " + caDETClass.Name + ".";
                    break;
                case "NOPF": // number of protected fields
                    if (metricValue < FromValue)
                        content = "Increase number of protected fields in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of protected fields in the class " + caDETClass.Name + ".";
                    break;
                case "CMNB": // max nested blocks from all class members
                    if (metricValue < FromValue)
                        content = "Increase max nested blocks from all class members in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease max nested blocks from all class members in the class " + caDETClass.Name + ".";
                    break;
                case "RFC": // response for a class, counts the number of unique method invocations
                    if (metricValue < FromValue)
                        content = "Increase number of unique method invocations in the class " + caDETClass.Name + ".";
                    else
                        content = "Decrease number of unique method invocations in the class " + caDETClass.Name + ".";
                    break;
                #endregion
                default:
                    content = MetricName + " is the issue.";
                    break;
            }

            return new ChallengeHint
            {
                Content = content,
                LearningObjectSummaryId = 337
            };
        }

        public ChallengeHint GetApplicableHintForIncompleteMethod(CaDETMember caDETMethod)
        {
            var metricValue = caDETMethod.Metrics[(CaDETMetric)Enum.Parse(typeof(CaDETMetric), MetricName, true)];
            string content;

            switch (this.MetricName)
            {
                #region MethodMetrics
                case "CYCLO": // cyclomatic complexity
                    if (metricValue < FromValue)
                        content = "Increase cyclomatic complexity in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease cyclomatic complexity in a method " + caDETMethod.Name + ".";
                    break;
                case "MLOC": // lines of code in a method
                    if (metricValue < FromValue)
                        content = "Increase lines of code in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease lines of code in a method " + caDETMethod.Name + ".";
                    break;
                case "MELOC": // effective lines of code of a method, excluding comments, blank lines, function header and function braces
                    if (metricValue < FromValue)
                        content = "Increase effective lines of code of a method (excluding comments, blank lines, function header and function braces) " + caDETMethod.Name + ".";
                    else
                        content = "Decrease effective lines of code of a method (excluding comments, blank lines, function header and function braces) " + caDETMethod.Name + ".";
                    break;
                case "NOP": // number of parameters
                    if (metricValue < FromValue)
                        content = "Increase number of parameters in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of parameters in a method " + caDETMethod.Name + ".";
                    break;
                case "NOLV": // number of local variables
                    if (metricValue < FromValue)
                        content = "Increase number of local variables in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of local variables in a method " + caDETMethod.Name + ".";
                    break;
                case "NOTC": // number of try catch blocks
                    if (metricValue < FromValue)
                        content = "Increase number of try catch blocks in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of try catch blocks in a method " + caDETMethod.Name + ".";
                    break;
                case "MNOL": // number of loops in method
                    if (metricValue < FromValue)
                        content = "Increase number of loops in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of loops in a method " + caDETMethod.Name + ".";
                    break;
                case "MNOR": // number of return statements in method
                    if (metricValue < FromValue)
                        content = "Increase number of return statements in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of return statements in a method " + caDETMethod.Name + ".";
                    break;
                case "MNOC": // number of comparison operators in method
                    if (metricValue < FromValue)
                        content = "Increase number of comparison operators in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of comparison operators in a method " + caDETMethod.Name + ".";
                    break;
                case "MNOA": // number of assignments in method
                    if (metricValue < FromValue)
                        content = "Increase number of assignments in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of assignments in a method " + caDETMethod.Name + ".";
                    break;
                case "NONL": // number of numeric literals
                    if (metricValue < FromValue)
                        content = "Increase number of numeric literals in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of numeric literals in a method " + caDETMethod.Name + ".";
                    break;
                case "NOSL": // number of string literals
                    if (metricValue < FromValue)
                        content = "Increase number of string literals in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of string literals in a method " + caDETMethod.Name + ".";
                    break;
                case "NOMO": // number of math operations
                    if (metricValue < FromValue)
                        content = "Increase number of math operations in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of math operations in a method " + caDETMethod.Name + ".";
                    break;
                case "NOPE": // number of parenthesized expressions
                    if (metricValue < FromValue)
                        content = "Increase number of parenthesized expressions in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of parenthesized expressions in a method " + caDETMethod.Name + ".";
                    break;
                case "NOLE": // number of lambda expressions
                    if (metricValue < FromValue)
                        content = "Increase number of lambda expressions in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of lambda expressions in a method " + caDETMethod.Name + ".";
                    break;
                case "MMNB": // max nested blocks in the method
                    if (metricValue < FromValue)
                        content = "Increase max nested blocks in the method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease max nested blocks in the method " + caDETMethod.Name + ".";
                    break;
                case "NOUW": // number of unique words
                    if (metricValue < FromValue)
                        content = "Increase number of unique words in a method " + caDETMethod.Name + ".";
                    else
                        content = "Decrease number of unique words in a method " + caDETMethod.Name + ".";
                    break;
                #endregion
                default:
                    content = MetricName + " is the issue.";
                    break;
            }

            return new ChallengeHint
            {
                Content = content,
                LearningObjectSummaryId = 337
            };
        }
    }
}
