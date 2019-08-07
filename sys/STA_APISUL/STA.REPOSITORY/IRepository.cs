using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STA.REPOSITORY
{
    public interface IRepository<T> where T : class
    {
        void Adicionar(T item);
        void Atualizar(T item);
        void Remover(T item);
        T BuscarPorId(object id);
        IQueryable<T> BuscarTodos();
    }
}
