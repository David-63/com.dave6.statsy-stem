using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace Core.Nodes
{
    public class DivideNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode dividend;
        [HideInInspector] public CodeFunctionNode divisor;

        public override float value => dividend.value / divisor.value;

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new();
                if (dividend != null)
                {
                    nodes.Add(dividend);
                }
                if (divisor != null)
                {
                    nodes.Add(divisor);
                }

                return nodes.AsReadOnly();
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                dividend = child;
            }
            else
            {
                divisor = child;
            }
        }

        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                dividend = null;
            }
            else
            {
                divisor = null;
            }
        }
    }
}