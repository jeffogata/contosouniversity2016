namespace ContosoUniversity.Domain
{
    public abstract class Person : Entity
    {
        public virtual string LastName { get; set; }

        public virtual string FirstName { get; set; }
    }
}