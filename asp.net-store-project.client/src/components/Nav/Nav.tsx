import { default as SearchBar } from './SearchBar';
import { useRef } from 'react';
import './Nav.css';
import { Link } from 'react-router-dom';

export function Nav() {
	const searchBarRef = useRef(null);
	const focusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.add("focused");
	const unfocusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.remove("focused");

	const menuRef = useRef(null);
	const openMenu = () => (menuRef.current! as HTMLElement).style.top = "60px";
	const hideMenu = () => (menuRef.current! as HTMLElement).style.top = "7px";

	const searchBarProps = {
		searchBarRef, 
		focusSearchBar, 
		unfocusSearchBar
	}

    return <nav onMouseLeave={hideMenu}>
		<div className="navigation">
			<div className="logo-section">
				<Link to="/">
					<img src="https://placehold.co/120x50" alt="logo" />
				</Link>
			</div>
			<div className="empty-space-section"></div>
			<SearchBar {...searchBarProps}/>
			<div className="menu-section">
				<Link to="/basket">
					<img src="https://placehold.co/40x40" alt="basket" />
				</Link>
				<img onMouseOver={openMenu} src="https://placehold.co/40x40" alt="profile" />
			</div>
		</div>
		<div className="menu" ref={menuRef}>
			<div className="empty-space"></div>
			<div className="option-section">
				<Link to="/sign-in">Sign in</Link>
			</div>
			<div className="option-section">
				<Link to="/sign-up">Sign up</Link>
			</div>
		</div>
	</nav>;
}

export default Nav;