namespace EllipticCurve.Interfaces
{
    public interface IFieldModel<T>
    {
        public T Subtract(T right);
        
        public T Multiply(T right);
        
        public T Divide(T right);
        
        public T Modulus(T right);
    }
}