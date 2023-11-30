namespace shared.Model
{
      public class Grupo : Model
      {
            public int? ordem { get => Get<int?>(); set => Set(value); }
            public string? Descricao { get => Get<string?>(); set => Set(value); }
      }
}
