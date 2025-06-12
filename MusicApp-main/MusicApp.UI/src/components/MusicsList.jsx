import { useEffect, useState, useRef } from "react";
import { useDispatch, useSelector } from "react-redux";
import { deleteMusicFetch, getMusicsFetch } from "../store/slices/music/musicFetchs";
import { addFavouriteFetch, getFavouritesFetch, removeFavouriteFetch } from "../store/slices/favourite/favouritesFetchs";

export default function MusicList() {
    const dispatch = useDispatch();
    const musicList = useSelector((state) => state.music?.musics || []);
    const favoriteMusicIds = useSelector((state) => state.favourite?.favourites || []);
    const audioRefs = useRef({});
    const [playingId, setPlayingId] = useState(null);
    const [pausedIds, setPausedIds] = useState({});
    const baseFileShareURL = import.meta.env.VITE_FILESHARE_API_URL;

    useEffect(() => {
        dispatch(getMusicsFetch());
        dispatch(getFavouritesFetch());
    }, []);

    const stopAll = () => {
        Object.values(audioRefs.current).forEach((audio) => {
            if (audio) {
                audio.pause();
                audio.currentTime = 0;
            }
        });
        setPlayingId(null);
        setPausedIds({});
    };

    const togglePlay = (id) => {
        const audio = audioRefs.current[id];
        if (!audio) return;
        if (playingId === id && !pausedIds[id]) {
            audio.pause();
            setPausedIds((prev) => ({ ...prev, [id]: true }));
        } else {
            stopAll();
            audio.play();
            setPlayingId(id);
            setPausedIds((prev) => ({ ...prev, [id]: false }));
        }
    };

    const toggleFavorite = (id) => {
        if (favoriteMusicIds.includes(id)) {
            dispatch(removeFavouriteFetch(id));
        } else {
            dispatch(addFavouriteFetch(id));
        }
    };

    const deleteMusic = (id) => {
        stopAll();
        dispatch(deleteMusicFetch(id));
    };

    return (
        <div style={{ backgroundColor: "#121212", color: "white", padding: "2rem", minHeight: "100vh" }}>
            <h2 style={{ color: "#1DB954", fontSize: "2rem", marginBottom: "1.5rem" }}>Your Music Library</h2>
            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fill, minmax(250px, 1fr))", gap: "1.5rem" }}>
                {musicList.map((music) => (
                    <div
                        key={music.id}
                        style={{
                            backgroundColor: "#181818",
                            padding: "1rem",
                            borderRadius: "10px",
                            display: "flex",
                            flexDirection: "column",
                            gap: "0.75rem",
                            alignItems: "center",
                            textAlign: "center"
                        }}
                    >
                        <img
                            src={`${baseFileShareURL}${music.photoPath}`}
                            alt={music.name}
                            style={{ width: "100%", height: "200px", objectFit: "cover", borderRadius: "8px" }}
                        />
                        <div style={{ fontWeight: "bold", fontSize: "1.1rem" }}>{music.name}</div>
                        <audio
                            ref={(el) => (audioRefs.current[music.id] = el)}
                            src={`${baseFileShareURL}${music.musicPath}`}
                            preload="auto"
                            style={{ width: "100%" }}
                        />
                        <div style={{ display: "flex", justifyContent: "center", gap: "1rem" }}>
                            <button
                                onClick={() => togglePlay(music.id)}
                                style={{ backgroundColor: "#1DB954", border: "none", padding: "0.5rem 1rem", color: "white", borderRadius: "20px" }}
                            >
                                {playingId === music.id && !pausedIds[music.id] ? "Pause" : "Play"}
                            </button>
                            <button
                                onClick={() => toggleFavorite(music.id)}
                                style={{ backgroundColor: "transparent", border: "none", fontSize: "1.5rem", cursor: "pointer" }}
                            >
                                {favoriteMusicIds.includes(music.id) ? "❤️" : "🤍"}
                            </button>
                            <button
                                onClick={() => deleteMusic(music.id)}
                                style={{ backgroundColor: "#ff4d4d", border: "none", padding: "0.5rem 1rem", color: "white", borderRadius: "20px" }}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
