using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Utilities;


namespace AST
{
    public abstract class Statement
    {
        public abstract string Unparse(int level = 0);
    }

    public class BlockStmt : Statement
    {
        public List<Statement> Statements { get; }

        public BlockStmt(List<Statement> statements)
        {
            Statements = statements;
        }

        public override string Unparse(int level = 0)
        {
            string indent = GeneralUtils.GetIndentation(level);
            string result = indent + "{\n";

            foreach (var lineblock in Statements)
            {
                result += lineblock.Unparse(level + 1) + "\n";
            }

            result += indent + "}";
            return result;
        }
    }

    public class AssignmentStmt : Statement
    {
        public VariableNode Variable { get; }
        public ExpressionNode Expression { get; }

        public AssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override string Unparse(int level = 0)
        {
            string indent = GeneralUtils.GetIndentation(level);
            return $"{indent}{Variable.Unparse()} := {Expression.Unparse()};";
        }
    }

    public class ReturnStmt : Statement
    {
        public ExpressionNode Expression { get; }

        public ReturnStmt(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override string Unparse(int level = 0)
        {
            string indent = GeneralUtils.GetIndentation(level);
            return $"{indent}return {Expression.Unparse()};";
        }
    }
    public abstract class ExpressionNode
    {
        public abstract string Unparse(int level = 0);
    }

    public class LiteralNode : ExpressionNode
    {
        public object Value { get; }

        public LiteralNode(object value)
        {
            Value = value;
        }

        public override string Unparse(int level = 0)
        {
            return Value.ToString();
        }
    }

    public class VariableNode : ExpressionNode
    {
        public string Name { get; set; }

        public VariableNode(string name)
        {
            Name = name;
        }

        public override string Unparse(int level = 0)
        {
            return Name;
        }
    }

    public abstract class Operator : ExpressionNode
    { }

    public abstract class BinaryOperator : Operator
    {
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        public BinaryOperator(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }

    public class PlusNode : BinaryOperator
    {
        public PlusNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} + {Right.Unparse(level)})";
        }
    }

    public class MinusNode : BinaryOperator
    {
        public MinusNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} - {Right.Unparse(level)})";
        }
    }

    public class TimesNode : BinaryOperator
    {
        public TimesNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} * {Right.Unparse(level)})";
        }
    }

    public class FloatDivNode : BinaryOperator
    {
        public FloatDivNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} / {Right.Unparse(level)})";
        }
    }

    public class IntDivNode : BinaryOperator
    {
        public IntDivNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} // {Right.Unparse(level)})";
        }
    }

    public class ModulusNode : BinaryOperator
    {
        public ModulusNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} % {Right.Unparse(level)})";
        }
    }

    public class ExponentiationNode : BinaryOperator
    {
        public ExponentiationNode(ExpressionNode left, ExpressionNode right)
            : base(left, right) { }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} ** {Right.Unparse(level)})";
        }
    }
}
