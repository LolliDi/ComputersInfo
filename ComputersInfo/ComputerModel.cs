using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputersInfo
{
    public class ComputerModel
    {
        Computers computer;
        Processors processor;
        List<VideoControllers> videoControllers = new List<VideoControllers>();
        List<PhysicalMemory> physicalMemories = new List<PhysicalMemory>();
        List<HardDrives> hardDrives = new List<HardDrives>();
        MotherBoards motherBoard = new MotherBoards();
        OS oS;
        #region getterSetter
        public Computers Computer 
        { 
            get => computer;
            set => computer = value;
        }
        public Processors Processor
        {
            get => processor;
            set => processor = value;
        }
        public List<VideoControllers> VideoControllers
        {
            get => videoControllers;
            set => videoControllers = value;
        }
        public List <PhysicalMemory> PhysicalMemory
        {
            get => physicalMemories;
            set => physicalMemories = value;
        }
        public List<HardDrives > HardDrives
        {
            get => hardDrives;
            set => hardDrives = value;
        }
        public MotherBoards MotherBoard
        {
            get => motherBoard;
            set => motherBoard = value;
        }
        public OS OS
        {
            get => oS;
            set => oS = value;
        }
        #endregion
        public ComputerModel(Computers comp)
        {
            computer = comp;
            int id = comp.id;
            if(computer!=null)
            {
                processor = DBCl.db.Processors.FirstOrDefault(x => x.Id == computer.ProcessorId);
                List<ComputersVideo> cv = DBCl.db.ComputersVideo.Where(x => x.IdPC == id).ToList();
                foreach (ComputersVideo c in cv)
                {
                    videoControllers.Add(DBCl.db.VideoControllers.FirstOrDefault(x => x.Id==c.IdVideo));
                }
                physicalMemories = DBCl.db.PhysicalMemory.Where(x=>x.IdPC==id).ToList();
                List<ComputerHard> ch = DBCl.db.ComputerHard.Where(x => x.IdPC == id).ToList();
                foreach(ComputerHard c in ch)
                {
                    hardDrives.Add(DBCl.db.HardDrives.FirstOrDefault(x => x.Id == c.Id));
                }
                motherBoard = DBCl.db.MotherBoards.FirstOrDefault(x => x.Id == computer.MotherBoardId);
                oS = DBCl.db.OS.FirstOrDefault(x => x.IdPC==id);
            }

        }
        
    }
}
