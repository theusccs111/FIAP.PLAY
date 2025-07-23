namespace FIAP.PLAY.Domain.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entidade \"{name}\" ({key}) não encontrada.")
        {
        }

        public NotFoundException(string name) : base($"{name}")
        {

        }
    }
}
