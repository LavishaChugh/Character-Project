namespace FirstProject.Dtos.Fight
{
    public class HighScoreDto
    {

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Fight { get; set; }

        public int victories { get; set; }

        public int Defeats { get; set; }
    }
}
