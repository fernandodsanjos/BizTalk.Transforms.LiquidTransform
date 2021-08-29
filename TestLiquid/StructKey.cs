using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLiquid
{
    struct TupleKey<M, S> : IEquatable<TupleKey<M, S>>
    {
        readonly M messageType;
        readonly S strongType;

        public TupleKey(M messageType, S stringMessageType)
        {
            this.messageType = messageType;
            this.strongType = stringMessageType;
        }

        public M MessageType { get { return messageType; } }
        public S StrongType { get { return strongType; } }
        

        public override int GetHashCode()
        {
            return first.GetHashCode() ^ second.GetHashCode() ^ third.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((Tuple2<T, U, W>)obj);
        }

        public bool Equals(Tuple2<T, U, W> other)
        {
            return other.first.Equals(first) && other.second.Equals(second) && other.third.Equals(third);
        }
    }
}
