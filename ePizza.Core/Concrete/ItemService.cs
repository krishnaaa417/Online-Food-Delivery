using AutoMapper;
using ePizza.Core.Contracts;
using ePizza.Models.Response;
using ePizza.Repository.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ePizza.Core.Concrete
{
    public class ItemService : IItemService
    {
        private readonly IItemRespository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ItemService(
            IItemRespository itemRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public IEnumerable<ItemResponseModel> GetItems()
        {
            var items = _itemRepository.GetAll();

            return _mapper.Map<IEnumerable<ItemResponseModel>>(items);   
        }

        public IEnumerable<ItemResponseModel> GetItemsUsingAdo()
        {
            List<ItemResponseModel> itemsList = new();
            using SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = _configuration.GetConnectionString("DatabaseConnection");
            sqlConnection.Open();


            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            //sqlCommand.CommandText = "select * from Items where Id = @Id";
            //sqlCommand.Parameters.AddWithValue("@Id", 1);

            sqlCommand.CommandText = "Select * from Items";


            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                ItemResponseModel itemsResponse = new ItemResponseModel();


                itemsResponse.ImageUrl = reader["ImageUrl"].ToString();
                itemsResponse.ItemTypeId = Convert.ToInt32(reader["ItemTypeId"]);
                itemsResponse.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);

                itemsList.Add(itemsResponse);
            }


            return itemsList;
        }

        public IEnumerable<ItemResponseModel> GetItemsUsingProcedure()
        {
            var items = _itemRepository.CallProcedure();

            return new List<ItemResponseModel>();
        }
    }
}
