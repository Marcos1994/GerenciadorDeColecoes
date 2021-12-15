using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MinhasColecoes.Persistencia.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Repositories
{
	public abstract class Repository : IRepository
	{
		protected IDbContextTransaction transacao;
		protected abstract DbContext GetContext();
		public void StartTransaction(string transactionName)
		{
			if (transacao == null)
				transacao = GetContext().Database.BeginTransaction();
			transacao.CreateSavepoint(transactionName);
		}
		public void RollbackTransaction(string transactionName)
		{
			if (transacao == null)
				throw new Exception("Tranzação não iniciada.");
			transacao.RollbackToSavepoint(transactionName);
		}
		public void FinishTransaction()
		{
			if (transacao == null)
				throw new Exception("Tranzação não iniciada.");
			transacao.Commit();
		}
	}
}
