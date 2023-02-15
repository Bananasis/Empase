
    public interface ICellPool
    {
        Cell GetCell(CellData cd);
        Player CellGetPlayer(CellData cd);
        void ReturnPlayer();
        void Return(Cell cell);
        Player Player { get; }
    }

    public partial class CellPool : ICellPool
    {
        public Player Player => player;
        public Cell GetCell(CellData cd)
        {
            var queue = pool[cd.type];
            if (queue.Count == 0)
            {
                queue.Enqueue(CreateCell(cd));
            }

            var cell = queue.Dequeue();
            cell.gameObject.SetActive(true);
            cd.size = cd.size;
            cell.cData = cd;
            cell.Move(cd.position);
            cell.transform.position = cd.position;
            
            return cell;
        }




        public Player CellGetPlayer(CellData cd)
        {
            player.gameObject.SetActive(true);
            cd.size = cd.size;
            player.cData = cd;
            player.Move(cd.position);
            player.transform.position = cd.position;
            return player;
        }

        public void ReturnPlayer()
        {
            player.gameObject.SetActive(false);
        }

        public void Return(Cell cell)
        {
            cell.gameObject.SetActive(false);
            pool[cell.cData.type].Enqueue(cell);
        }
    }