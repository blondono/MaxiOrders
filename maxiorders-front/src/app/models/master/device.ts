export class Device {
    constructor(
        public idDevice: number,
        public name: string,
        public brand: string,
        public model: string,
        public serie: string,
        public licenseNumber: string,
        public state: boolean,
        public manufacturingDate: string,
        public purchaseDate: string,
        public instalationDate: string,
        public image: string,
        public billImage: string,
        public dataSheets: string
    ){}
}