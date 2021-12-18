using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IRepository
	{
		/// <summary>
		/// Inicia uma transação.
		/// </summary>
		/// <param name="transactionName"></param>
		void StartTransaction(string transactionName);

		/// <summary>
		/// Reverte uma transação.
		/// </summary>
		/// <param name="transactionName"></param>
		void RollbackTransaction(string transactionName);

		/// <summary>
		/// Encerra as transações abertas.
		/// </summary>
		void FinishTransaction();
	}
}
