using System.Linq.Expressions;

namespace PracticasExpresionFuncionesWebApi.Repository
{
    public class GenericRepository<T> where T : class
    {

        private readonly List<T> _data = new();

        public void Add(T item)
        {
            _data.Add(item);
        }

        public List<T> GetAll()
        {
            return _data;
        }

        // Uso de expresiones
        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _data.AsQueryable().Where(predicate).ToList();
        }


    }
}
