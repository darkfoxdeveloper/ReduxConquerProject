using NHibernate.Criterion;
using NHibernate.Transform;
using Redux.Database.Domain;

namespace Redux.Database.Repositories
{
    public class GuildRepository : Repository<uint, DbGuild>
    {
        public IList<DbGuildMemberInfo> GetMemberInfo(int guildId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .GetNamedQuery("GetGuildMemberList")
                    .SetParameter("guildId", guildId)
                    .SetResultTransformer(Transformers.AliasToBean<DbGuildMemberInfo>())
                    .List<DbGuildMemberInfo>();
            }
        }

        public DbGuild GetGuildByName(string name)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbGuild>()
                    .Add(Restrictions.Eq("Name", name))
                    .UniqueResult<DbGuild>();
            }
        }

        public IList<DbGuild> GetAll()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session
                    .CreateCriteria<DbGuild>()
                    .List<DbGuild>();
            }
        }
    }
}

