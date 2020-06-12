using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebCore.Common;

namespace Core.Utils
{
    public enum OperandType
    {
        NAME,
        VALUE
    }

    public class Operand
    {
        public OperandType Type { get; set; }
        public string NameOrValue { get; set; }

        public Operand(OperandType type, string nameOrValue)
        {
            Type = type;
            if(type == OperandType.NAME)
                NameOrValue = nameOrValue.Substring(1);
            else
                NameOrValue = nameOrValue;
        }
    }

    public class ProcExpression
    {
        public string StoreProcName { get; set; }
        public List<Operand> Operands { get; set; }

        public ProcExpression()
        {
            Operands = new List<Operand>();
        }
    }

    public class ExpressionUtils
    {
        public static bool IsExpression(string source)
        {
            return Regex.IsMatch(source, "([^\\(]+)\\(([^\\)]*)\\)");
        }

        public static ProcExpression ParseScript(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                var procExpression = new ProcExpression();
                var match = Regex.Match(source, "([^\\(]+)\\(([^\\)]*)\\)");

                if (match.Success)
                {
                    procExpression.StoreProcName = match.Groups[1].Value;
                    //
                    var strParams = match.Groups[2].Value + ",";
                    if (!string.IsNullOrEmpty(match.Groups[2].Value))
                    {
                        var cur = 0;
                        var opened = false;
                        string param = null;
                        string value = null;

                        while (cur < strParams.Length)
                        {
                            if (strParams[cur] == '\'')
                            {
                                if (cur + 1 < strParams.Length && strParams[cur + 1] == '\'' && opened)
                                {
                                    value += '\'';
                                    cur++;
                                }
                                else
                                {
                                    opened = !opened;
                                }
                            }
                            else if (strParams[cur] == ',')
                            {
                                if (opened)
                                {
                                    value += ',';
                                }
                                else
                                {
                                    decimal num;
                                    param = param == null ? param : param.Trim();
                                    if (string.IsNullOrEmpty(param))
                                    {
                                        procExpression.Operands.Add(new Operand(OperandType.VALUE, value));
                                    }
                                    else if (decimal.TryParse(param, out num))
                                    {
                                        procExpression.Operands.Add(new Operand(OperandType.VALUE, num.ToString()));
                                    }
                                    else if (value == null)
                                    {
                                        procExpression.Operands.Add(new Operand(OperandType.NAME, param));
                                    }
                                    else
                                    {
                                        //throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_PARSE_SQL_SCRIPT_ERROR,
                                        //    "ParseScript", source);
                                    }

                                    param = null;
                                    value = null;
                                }
                            }
                            else
                            {
                                if (opened)
                                {
                                    value += strParams[cur];
                                }
                                else
                                {
                                    param += strParams[cur];
                                }
                            }
                            cur++;
                        }
                    }

                    return procExpression;
                }

                //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_PARSE_SQL_SCRIPT_ERROR,
                //                             "ParseScript", source);
            }
            return null;
        }
    }
}
