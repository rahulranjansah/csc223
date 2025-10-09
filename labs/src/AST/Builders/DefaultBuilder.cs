using System;
using System.Collections.Generic;

namespace AST
{
    /// <summary>
    /// DefaultBuilder constructs AST nodes and prints simple trace messages.
    /// Methods are virtual so DebugBuilder and NullBuilder can override them.
    /// To override we need base method to be override, virtual, or abstract
    /// </summary>
    public class DefaultBuilder
    {
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating PlusNode");
            return new PlusNode(left, right);
        }

        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating MinusNode");
            return new MinusNode(left, right);
        }

        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating TimesNode");
            return new TimesNode(left, right);
        }

        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating FloatDivNode");
            return new FloatDivNode(left, right);
        }

        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating IntDivNode");
            return new IntDivNode(left, right);
        }

        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating ModulusNode");
            return new ModulusNode(left, right);
        }

        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("DefaultBuilder: creating ExponentiationNode");
            return new ExponentiationNode(left, right);
        }

        public virtual LiteralNode CreateLiteralNode(object value)
        {
            Console.WriteLine("DefaultBuilder: creating LiteralNode");
            return new LiteralNode(value);
        }

        public virtual VariableNode CreateVariableNode(string name)
        {
            Console.WriteLine("DefaultBuilder: creating VariableNode");
            return new VariableNode(name);
        }

        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Console.WriteLine("DefaultBuilder: creating AssignmentStmt");
            return new AssignmentStmt(variable, expression);
        }

        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            Console.WriteLine("DefaultBuilder: creating ReturnStmt");
            return new ReturnStmt(expression);
        }

        public virtual BlockStmt CreateBlockStmt(List<Statement> statements)
        {
            Console.WriteLine("DefaultBuilder: creating BlockStmt");
            return new BlockStmt(statements);
        }
    }
}
