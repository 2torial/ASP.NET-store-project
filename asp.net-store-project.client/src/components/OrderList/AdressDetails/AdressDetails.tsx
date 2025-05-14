import { AdressInfo } from '../../../shared/StoreObject/OrderInfo';
import './AdressDetails.css';

interface AdressDetailsProps {
    details: AdressInfo;
}

function AdressDetails({ details }: AdressDetailsProps) {
    return <table className="adress-details">
        <tr>
            <th colSpan={2}>Adress details</th>
        </tr>
        <tr>
            <td>Region</td>
            <td>{details.region}</td>
        </tr>
        <tr>
            <td>City</td>
            <td>{details.city}</td>
        </tr>
        <tr>
            <td>Postal code</td>
            <td>{details.postalCode}</td>
        </tr>
        <tr>
            <td>Street</td>
            <td>{details.streetName}</td>
        </tr>
        <tr>
            <td>House number</td>
            <td>{details.houseNumber}</td>
        </tr>
        <tr>
            <td>Apartment number</td>
            <td>{details.apartmentNumber}</td>
        </tr>
    </table>;
}

export default AdressDetails;