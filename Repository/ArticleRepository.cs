using IRepository;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public class ArticleRepository : IArticleRepository
    {
        public void Add(Article model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Article model)
        {
            throw new NotImplementedException();
        }

        public List<Article> Query(Expression<Func<Article, bool>> whereExpression)
        {
            throw new NotImplementedException();
        }

        public void Update(Article model)
        {
            throw new NotImplementedException();
        }


    }//Class_end
}
