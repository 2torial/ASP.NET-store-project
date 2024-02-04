import { useRef, useState } from "react";
import "./SearchBar.css";
import { useNavigate } from "react-router-dom";

function SearchBar() {
    const searchBarRef = useRef(null);
    const focusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.add("focused");
    const unfocusSearchBar = () => (searchBarRef.current! as HTMLElement).classList.remove("focused");
    const [searchBarInput, setSearchBar] = useState("");
    const navigate = useNavigate();

    const search = (event: React.SyntheticEvent) => {
        event.preventDefault();
        navigate({
            pathname: "/store",
            search: `?search=${searchBarInput}`,
        });
    };

    return <div className="search-bar-section">
        <form autoComplete="off" onSubmit={search}>
            <search className="search-bar" ref={searchBarRef} onFocus={focusSearchBar} onBlur={unfocusSearchBar}>
                <input className="input-area" type="text" name="SearchBar" placeholder="Search"
                    value={searchBarInput} onChange={e => setSearchBar(e.target.value)} />
                <input className="search-button" type="image" src="https://placehold.co/40x40" alt="magnifier" />
            </search>
        </form>
    </div>;
}

export default SearchBar;