function PessoaFisica(Cpf, NomeCompleto, DataNascimento, Sexo, Telefone) {
    this.Cpf = Cpf;
    this.NomeCompleto = NomeCompleto;
    this.DataNascimento = DataNascimento;
    this.Sexo = Sexo;
    this.Telefone = Telefone;
}

function PessoaJuridica(Cnpj, RazaoSocial, NomeFantasia, Segmento, Telefone) {
    this.Cnpj = Cnpj;
    this.RazaoSocial = RazaoSocial;
    this.NomeFantasia = NomeFantasia;
    this.Segmento = Segmento;
    this.Telefone = Telefone;
}

function Endereco(CEP, Municipio, UF, Logradouro, Numero, Bairro) {
    this.CEP = CEP;
    this.Municipio = Municipio;
    this.UF = UF;
    this.Bairro = Bairro;
    this.Numero = Numero;
    this.Logradouro = Logradouro;
}

function Usuario(Username, Password, Email, RepitaPassword, TipoUsuario) {
    this.Username = Username;
    this.Password = Password;
    this.Email = Email;
    this.RepitaPassword = RepitaPassword;
    this.TipoUsuario = TipoUsuario;
}