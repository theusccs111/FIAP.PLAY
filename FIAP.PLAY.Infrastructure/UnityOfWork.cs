﻿using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Shared.Entities;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Infrastructure.Data;
using FIAP.PLAY.Infrastructure.Repositories;

namespace FIAP.PLAY.Infrastructure
{
    public class UnityOfWork : IUnityOfWork, IDisposable
    {
        private readonly FiapPlayContext _context;
        private Dictionary<string, object> repositories;

        public IRepository<Usuario> Users { get { return new Repository<Usuario>(_context); } }
        public IRepository<Jogo> Jogos { get { return new Repository<Jogo>(_context); } }

        public UnityOfWork(FiapPlayContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> Repository<T>() where T : EntidadeBase
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositorioType = typeof(Repository<>);
                var repositorioInstancia = Activator.CreateInstance(repositorioType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositorioInstancia);
            }

            return (Repository<T>)repositories[type];
        }
    }
}
