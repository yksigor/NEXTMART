using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Validate
{
    public class CEPAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cep = Convert.ToString(value);

            if (String.IsNullOrEmpty(cep))
                return true;

            return Validacao.Cep(cep);
        }
    }

    public class CPFAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is null)
            {
                base.ErrorMessage = "Informe um CPF!";
                return false;
            }

            var cpf = Convert.ToString(value);

            if (String.IsNullOrEmpty(cpf))
                return true;

            if (!Validacao.Cpf(cpf))
            {
                base.ErrorMessage = "CPF inválido";
                return false;
            }

            return true;
        }
    }

    public class CNPJAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cnpj = Convert.ToString(value);

            if (String.IsNullOrEmpty(cnpj))
                return true;

            return Validacao.Cnpj(cnpj);
        }
    }

}
