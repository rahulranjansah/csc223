using System;
using System.Collections.Generic;
using Utilities.Containers;


namespace AST
{
    public abstract class Statement
    {
    public abstract string Unparse(int level = 0);
    }

    public class BlockStmt : Statement
    {
        public List<Statement> statement { get; }
        public BlockStmt(List<Statement> statement)
        {
            Statement = statement;
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
    }
    public class ReturnStmt : Statement
    {
        public ExpressionNode Expression { get; }
        public ReturnStmt(ExpressionNode expression)
        {
            Expression = expression;
        }
    }

    public abstract class ExpressionNode
    {
        public abstract string Unparse(int level = 0);
    }

    public class LiteralNode : ExpressionNode
    {
        // property
        public object Value { get; }
        // constructor
        protected LiteralNode(object value)
        {
            Value = value;
        }
    }
    public class VariableNode : ExpressionNode
    {
        // property
        public string Name { get; set; }
        protected VariableNode(string name)
        {
            Name = name;
        }
    }

    public abstract class Operator : ExpressionNode { }
    public abstract class BinaryOperator : Operator
    {
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }

        protected BinaryOperator(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }

    public class PlusNode : BinaryOperator
    {
        // constructor for abstract class, calls base BinaryOperator constructor
        public PlusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
    public class MinusNode : BinaryOperator
    {
        public MinusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }


    public class TimesNode : BinaryOperator
    {
        public TimesNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
    public class FloatDivNode : BinaryOperator
    {
        public FloatNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
    public class IntDivNode : BinaryOperator
    {
        public IntDivNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
    public class ModulusNode : BinaryOperator
    {
        public ModulusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
    public class ExponentiationNode : BinaryOperator
    {
        public ExponentiationNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { }
    }
}