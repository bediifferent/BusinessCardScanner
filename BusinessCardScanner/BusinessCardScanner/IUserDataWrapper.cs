using System.Collections.Generic;

namespace BusinessCardScanner
{
    public interface IUserDataWrapper
    {
        IList<T> LoadAll<T>() where T : new();

        void Insert(object item);

        void Update(object item);

        void Delete(object item);
    }
}