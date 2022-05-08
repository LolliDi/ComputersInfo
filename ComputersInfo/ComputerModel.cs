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
        double memoryCountMB;
        double memoryHardGB;
        public VideoControllers VC = null;
        #region getterSetter
        public double MemoryCountMB
        {
            get => memoryCountMB;
            set => memoryCountMB = value;
        }
        public double MemoryHardGB
        {
            get => memoryHardGB;
            set => memoryHardGB = value;
        }
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
            set
            {
                
                videoControllers = value;
                double mem = Convert.ToDouble(videoControllers[0].AdapterRAMMB);
                for (int i=0;i<videoControllers.Count();i++)
                {
                    if(mem<videoControllers[i].AdapterRAMMB)
                    {
                        mem = Convert.ToDouble(videoControllers[i].AdapterRAMMB);
                        VC = videoControllers[i];
                    }
                }
            }
        }
        public List <PhysicalMemory> PhysicalMemory
        {
            get => physicalMemories;
            set
            {
                physicalMemories = value;
                MemoryCountMB = 0;
                foreach (var p in physicalMemories)
                {
                    memoryCountMB += Convert.ToDouble(p.SizeMB);
                }
            }
        }
        public List<HardDrives> HardDrives
        {
            get => hardDrives;
            set
            {
                hardDrives = value;
                MemoryHardGB = 0;
                foreach (var p in hardDrives)
                {
                    memoryHardGB += Convert.ToDouble(p.SizeGB);
                }
            }
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
                    VideoControllers.Add(DBCl.db.VideoControllers.FirstOrDefault(x => x.Id==c.IdVideo));
                }
                PhysicalMemory = DBCl.db.PhysicalMemory.Where(x=>x.IdPC==id).ToList();
                List<ComputerHard> ch = DBCl.db.ComputerHard.Where(x => x.IdPC == id).ToList();
                foreach(ComputerHard c in ch)
                {
                    HardDrives.Add(DBCl.db.HardDrives.FirstOrDefault(x => x.Id == c.IdHard));
                }
                motherBoard = DBCl.db.MotherBoards.FirstOrDefault(x => x.Id == computer.MotherBoardId);
                oS = DBCl.db.OS.FirstOrDefault(x => x.IdPC==id);
            }

        }
        
    }
}
