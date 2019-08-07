using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using STA.MODEL.Models;

namespace STA.REPOSITORY
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private readonly DB_STAContext _contexto;

        /// <summary>
        /// Construtor da classe base de repositorio, verifica se a instância do banco está aberta, e caso não abre e inicia
        /// </summary>
        /// <param name="pcontexto">Intância do banco para ser iniciada</param>
        public Repository(DB_STAContext pcontexto)
        {
            if (pcontexto == null)
                this._contexto = new DB_STAContext();

            else
                this._contexto = pcontexto;
        }

        public Repository()
        {
            if (this._contexto == null)
                this._contexto = new DB_STAContext();
        }

        /// <summary>
        /// Função para inserir um objeto no banco
        /// </summary>
        /// <param name="item">Objeto a ser adicionado</param>
        public virtual void Adicionar(T item)
        {
            try
            {
                _contexto.Set<T>().Add(item);
                _contexto.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }

        public virtual T AdicionarRetornarObjetoInserido(T item)
        {
            try
            {
                _contexto.Set<T>().Add(item);
                _contexto.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                return item;
            }

        }

        /// <summary>
        /// Função que atualiza as informações de um objeto no banco
        /// </summary>
        /// <param name="item">objeto a ser atualizado</param>
        public virtual void Atualizar(T item)
        {
            try
            {
                //_contexto.Entry(item).State = EntityState.Modified;
                _contexto.Set<T>().AddOrUpdate(item);
                _contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Recupera todos os registros do modelo(tabela)
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> BuscarTodos()
        {

            return _contexto.Set<T>();
        }

        /// <summary>
        /// Função para remover objetos do banco
        /// </summary>
        /// <param name="item">objeto a ser removido</param>
        public virtual void Remover(T item)
        {
            try
            {
                _contexto.Set<T>().Remove(item);
                _contexto.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Função para buscar objetos por ID
        /// </summary>
        /// <param name="id"> Id do objeto</param>
        /// <returns></returns>
        public virtual T BuscarPorId(object id)
        {
            return _contexto.Set<T>().Find(id);
        }

        /// <summary>
        /// Fecha a conexão com o banco
        /// </summary>
        public void Dispose()
        {
        }
    }
}

