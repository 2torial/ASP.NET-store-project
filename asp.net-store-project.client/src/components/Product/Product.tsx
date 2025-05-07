import { useLocation } from 'react-router-dom';
import './Product.css';
import { useEffect, useState } from 'react';

interface ProductState {
	supplierId: string;
	productId: string;
}

function Product() {
	const [pageContent, setPageContent] = useState<string>();

	const location = useLocation();
	const state: ProductState = location.state;

	const readData = async () => {
		const response = await fetch(`/api/store/product/${state.supplierId}/${state.productId}`);
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		setPageContent(await response.text());
	}

	useEffect(() => {
		readData();
	}, []);

	return <main id="product-page">{pageContent}</main>;
}

export default Product;