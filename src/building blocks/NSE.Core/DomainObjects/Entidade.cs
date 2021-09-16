using System;

namespace NSE.Core.DomainObjects {
    public abstract class Entidade {
        public Guid Id { get; set; }

        public override bool Equals(object obj) {
            var compareTo = obj as Entidade;
            
            if(ReferenceEquals(this, compareTo)) return true;
            if(ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override string ToString() {
            return $"{GetType().Name} [Id: {Id}]";
        }

        public static bool operator !=(Entidade a, Entidade b) {
            return !(a == b);
        }

        public static bool operator ==(Entidade a, Entidade b) { 
            return a.Equals(b);
        }

        public override int GetHashCode() {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
    }
}
