namespace _Project._Common.Scripts.Contracts.Data
{
    public record SnapshotPhysicBody2D
    {
        public SnapshotPhysicBody2D(float mass,  float linearDamping, float angularDamping)
        {
            Mass =  mass;
            LinearDamping = linearDamping;
            AngularDamping = angularDamping;
        }
        
        public float AngularDamping { get; }
        public float LinearDamping { get; }
        public float Mass { get; }
    }
}