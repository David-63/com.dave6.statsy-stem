using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace Core.Nodes
{
    public class PowerNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode exponent;
        [HideInInspector] public CodeFunctionNode @base;

        public override float value => (float)Math.Pow(@base.value, exponent.value);

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new();
                if (exponent != null)
                {
                    nodes.Add(exponent);
                }
                if (@base != null)
                {
                    nodes.Add(@base);
                }

                return nodes.AsReadOnly();
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                exponent = child;
            }
            else
            {
                @base = child;
            }
        }

        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                exponent = null;
            }
            else
            {
                @base = null;
            }
        }
    }
}