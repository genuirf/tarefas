
namespace shared.Model
{
      public class Tarefa : Model
      {
            public int? ordem { get => Get<int?>(); set => Set(value); }
            public string? Titulo { get => Get<string?>(); set => Set(value); }
            public string? Descricao { get => Get<string?>(); set => Set(value); }
            public int? grupo_Id { get => Get<int?>(); set => Set(value); }
            public DateTime? DataCadastro { get => Get<DateTime?>(); set => Set(value); }
            public DateTime? DataConclusao { get => Get<DateTime?>(); set => Set(value); }
            public bool concluido { get => Get<bool>(); set => Set(value); }
            public bool arquivado { get => Get<bool>(); set => Set(value); }

            public override string ToString()
            {
                  return $"{Titulo} -> {Descricao}";
            }
      }
}
