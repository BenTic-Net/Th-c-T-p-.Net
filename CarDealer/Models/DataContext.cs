using System;
using System.Linq;
using FrameLog.Contexts;
using FrameLog.Models;

using FrameLog.Filter;
using FrameLog.History;
using System.Data.Entity;
using FrameLog;

namespace CarDealer.Models
{/*
    public class DataContext:DbContext
    {
        
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
            base.OnModelCreating(modelBuilder);
        }

      

        public DataContext(Action<DbContext> customSaveChangesLogic = null, ILoggingFilterProvider filterProvider = null):base("QuanLiOtoEntities1")
        {
            Database.SetInitializer<DataContext>(new ExampleContextInitializer());
            Logger = new FrameLogModule<ChangeSet, User>(new ChangeSetFactory(), FrameLogContext, filterProvider);
            CustomSaveChangesLogic = customSaveChangesLogic;
        }

        
        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; }

        public readonly FrameLogModule<ChangeSet, User> Logger;
        public IFrameLogContext<ChangeSet, User> FrameLogContext
        {
            get { return new MyContextAdapter(this); }
        }
        public HistoryExplorer<ChangeSet, User> HistoryExplorer
        {
            get { return new HistoryExplorer<ChangeSet, User>(FrameLogContext); }
        }

        public override int SaveChanges()
        {
            // This is an example of custom logic overriden within the EntityFramework's vanilla DbContext.SaveChanges().
            // Some developers have hooks here to perform various checks against their domain model during saves.
            // We want to make sure any code overridden here is called when using FrameLog in order to maintain a 
            // similar experereience as one would get when using vanilla EntityFramework.
            if (CustomSaveChangesLogic != null)
                CustomSaveChangesLogic(this);
            
            
            return base.SaveChanges();
        }


        public ISaveResult<ChangeSet> Save(User author)
        {
            // NOTE: This will eventually circle back and call our overridden SaveChanges() later
            return Logger.SaveChanges(author);
        }

        public Action<DbContext> CustomSaveChangesLogic { get; set; }



    }

    public class MyContextAdapter : DbContextAdapter<ChangeSet, User>
    {
        private DataContext context;

        public MyContextAdapter(DataContext context)
            : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<User>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<User>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<User>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }
        public override Type UnderlyingContextType
        {
            get { return typeof(DataContext); }
        }
    }

    public class ExampleContextInitializer : IDatabaseInitializer<DataContext>
    {
        private DropCreateDatabaseIfModelChanges<DataContext> wrapped;

        public ExampleContextInitializer()
        {
            wrapped = new DropCreateDatabaseIfModelChanges<DataContext>();
        }

        public void InitializeDatabase(DataContext context)
        {
            string databaseName = context.Database.Connection.Database;
            wrapped.InitializeDatabase(context);
            context.Database.ExecuteSqlCommand(
                TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE [{0}] SET READ_COMMITTED_SNAPSHOT ON", databaseName));
            context.Database.ExecuteSqlCommand(
                TransactionalBehavior.DoNotEnsureTransaction,
                string.Format("ALTER DATABASE [{0}] SET ALLOW_SNAPSHOT_ISOLATION ON", databaseName));
        }
    }
    */
}