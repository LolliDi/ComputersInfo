using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputersInfo
{
    public class ComputerModel
    {
        Computers computer;
        Processors processor;
        List<VideoControllers> videoController = new List<VideoControllers>();
        List<PhysicalMemory> physicalMemorie = new List<PhysicalMemory>();
        List<HardDrives> hardDrive = new List<HardDrives>();
        MotherBoards motherBoard = new MotherBoards();
        OS oS;
        double memoryCountMB;
        double memoryHardGB;
        VideoControllers vc;
        #region getterSetter
        public VideoControllers VC
        {
            get => vc;
            set => vc = value;
        }
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
        public List<VideoControllers> VideoController
        {
            get => videoController;
            set
            {

                videoController = value;
                double mem = Convert.ToDouble(videoController[0].AdapterRAMMB);
                vc = videoController[0];
                for (int i = 0; i < videoController.Count(); i++)
                {
                    if (mem < videoController[i].AdapterRAMMB)
                    {
                        mem = Convert.ToDouble(videoController[i].AdapterRAMMB);
                        vc = videoController[i];
                    }
                }
            }
        }
        public List<PhysicalMemory> PhysicalMemory
        {
            get => physicalMemorie;
            set
            {
                physicalMemorie = value;
                MemoryCountMB = 0;
                foreach (var p in physicalMemorie)
                {
                    memoryCountMB += Convert.ToDouble(p.SizeMB);
                }
            }
        }
        public List<HardDrives> HardDrive
        {
            get => hardDrive;
            set
            {
                hardDrive = value;
                MemoryHardGB = 0;
                foreach (var p in hardDrive)
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
            if (computer != null)
            {
                processor = DBCl.db.Processors.FirstOrDefault(x => x.Id == computer.ProcessorId);
                List<ComputersVideo> cv = DBCl.db.ComputersVideo.Where(x => x.IdPC == id).ToList();
                List<VideoControllers> v = new List<VideoControllers>();
                foreach (ComputersVideo c in cv)
                {
                    v.Add(DBCl.db.VideoControllers.FirstOrDefault(x => x.Id == c.IdVideo));
                }
                VideoController = v;
                PhysicalMemory = DBCl.db.PhysicalMemory.Where(x => x.IdPC == id).ToList();
                List<ComputerHard> ch = DBCl.db.ComputerHard.Where(x => x.IdPC == id).ToList();
                List<HardDrives> h = new List<HardDrives>();
                foreach (ComputerHard c in ch)
                {
                    h.Add(DBCl.db.HardDrives.FirstOrDefault(x => x.Id == c.IdHard));
                }
                HardDrive = h;
                motherBoard = DBCl.db.MotherBoards.FirstOrDefault(x => x.Id == computer.MotherBoardId);
                oS = DBCl.db.OS.FirstOrDefault(x => x.IdPC == id);
            }

        }

    }
}
