export class Device {
    constructor(
        public IdDevice: number,
        public Name: string,
        public Brand: string,
        public Model: string,
        public Serie: string,
        public LicenseNumber: string,
        public State: boolean,
        public ManufacturingDate: Date,
        public PurchaseDate: Date,
        public InstalationDate: Date,
        public Image: string,
        public BillImage: string,
        public DataSheets: string
    ){}
}