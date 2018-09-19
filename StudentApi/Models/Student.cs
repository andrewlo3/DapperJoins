namespace StudentApi.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Gpa { get; set; }

        public override string ToString()
        {
            return $"{Id},{Name},{Gpa}";
        }
    }
}