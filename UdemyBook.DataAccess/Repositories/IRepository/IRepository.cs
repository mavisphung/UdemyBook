using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UdemyBook.DataAccess.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id); //Lay61 object bằng id

        /// <summary>
        /// Trả về 1 danh sách có kiểu T.
        /// Tham số 1 là filter,
        /// tham số 2 là xếp theo tăng hoặc giảm,
        /// tham số 3 là dùng để lấy foreign key của đối tượng
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        /// <summary>
        /// Trả về 1 đối tượng T được lấy về từ Db thông qua IRepository.
        /// Tham số đầu tiên là Expression của LINQ,
        /// Tham số thứ 2 là dùng để lấy foreign key của đối tượng T
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        /// <summary>
        /// Dùng để thêm 1 entity vào db
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Xóa đối tượng dựa vào id
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        /// Xóa đối tượng dựa theo object
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Xóa nhiều đối tượng 1 lúc
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<T> entities);

    }
}
