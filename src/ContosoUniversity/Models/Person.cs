namespace ContosoUniversity.Models
{
    public abstract class Person : Entity
    {
        public virtual string LastName { get; set; }

        public virtual string FirstName { get; set; }

        public string FullName
        {
            get { return $"{LastName}, {FirstName}"; }
        }
    }
}