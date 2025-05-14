import { CustomerInfo } from '../../../shared/StoreObject/OrderInfo';
import './CustomerDetails.css';

interface CustomerDetailsProps {
    details: CustomerInfo;
}

function CustomerDetails({ details }: CustomerDetailsProps) {
    return <table className="customer-details">
        <tr>
            <th colSpan={2}>Customer details</th>
        </tr>
        <tr>
            <td>Name</td>
            <td>{details.name}</td>
        </tr>
        <tr>
            <td>Surname</td>
            <td>{details.surname}</td>
        </tr>
        <tr>
            <td>Phone</td>
            <td>{details.phoneNumber}</td>
        </tr>
        <tr>
            <td>Email</td>
            <td>{details.email}</td>
        </tr>
    </table>;
}

export default CustomerDetails;