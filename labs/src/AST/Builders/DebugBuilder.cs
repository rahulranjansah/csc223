using System;
using System.Collections.Generic;

namespace AST
{
    /// <summary>
    /// NullBuilder that does not create any objects; useful for assessing parsing problems
    /// while avoiding object creation
    /// </summary>
    /// This is the meat of the default building process write clean builders get rid of override.
    public class DebugBuilder : DefaultBuilder
    {
        // Override all creation methods to return null
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreatePlusNode(left, right);
        }

        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateMinusNode(left, right);
        }

        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateTimesNode(left, right);
        }

        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateFloatDivNode(left, right);
        }

        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateIntDivNode(left, right);
        }

        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateModulusNode(left, right);
        }

        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {

            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return base.CreateExponentiationNode(left, right);
        }

        public override LiteralNode CreateLiteralNode(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullExpception("value null");
            }
            return base.CreateLiteralNode(value);
        }

        public override VariableNode CreateVariableNode(string name)
        {
            if (name == null) { throw new ArgumentNullException("name error"); }
            return base.CreateVariableNode(name);
        }

        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            if (variable == null || expression == null)
            {
                throw new ArugmentNullException("either your variable or expression is null");
            }
            return base.CreateAssignmentStmt(variable, expression);
        }

        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            if (expression == null) { throw new ArgumentNullException("expression errors"); }
            return base.CreateReturnStmt(expression);
        }

        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            if (symbolTable == null) { throw new ArugmentNullException("symbol table string, object is off"); }
        }
        return base.CreateBlockStmt(symboltable);
    }
}