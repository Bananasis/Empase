
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
            cell.cData = cd;
            cell.gameObject.SetActive(true);
            cell.transform.position = cd.position;
            cell.Move(cd.position);
            return cell;
        }
        
        public Player CellGetPlayer(CellData cd)
        {
            player.cData = cd;
            player.playerData.defaultMassDefect = cd.cellMass.massDefect;
            player.gameObject.SetActive(true);
            player.transform.position = cd.position;
            player.Move(cd.position);
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