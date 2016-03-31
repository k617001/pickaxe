﻿/* Copyright 2015 Brock Reeve
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Pickaxe.Runtime;
using Pickaxe.Sdk;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pickaxe.CodeDom.Visitor
{
    public partial class CodeDomGenerator : IAstVisitor
    {
        public void Visit(VariableAssignmentStatement statement)
        {
            var leftArgs = VisitChild(statement.Left);
            var rightArgs = VisitChild(statement.Right);

            Type leftType = Type.GetType(leftArgs.Scope.CodeDomReference.BaseType);
            Type rightType = Type.GetType(rightArgs.Scope.CodeDomReference.BaseType);
            if (leftType != rightType && leftType != null)
            {
                var leftPrimitive = TablePrimitive.FromType(leftType);
                rightArgs.CodeExpression = leftPrimitive.ToNative(rightArgs.CodeExpression);
            }

            var assignment = new CodeAssignStatement(leftArgs.CodeExpression, rightArgs.CodeExpression);

            _codeStack.Peek().Scope = rightArgs.Scope;
            _codeStack.Peek().ParentStatements.Add(assignment);
        }
    }
    
}
