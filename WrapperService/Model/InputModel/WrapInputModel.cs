namespace WrapperService.Model.InputModel
{
    public class WrapInputModel
    {
        public IEnumerable<object> Data { get; set; }

        public WrapInputModel()
        {

        }

        public WrapInputModel(IEnumerable<object> data)
        {
            Data = data;
        }
    }
}
