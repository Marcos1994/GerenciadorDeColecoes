using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhasColecoes.Persistencia.Interfaces
{
	public interface IRepository
	{
		void StartTransaction(string transactionName);
		void RollbackTransaction(string transactionName);
		void FinishTransaction();
	}
}
