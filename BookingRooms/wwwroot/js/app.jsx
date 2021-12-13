class Room extends React.Component {

    constructor(props) {
        super(props);
        this.state = { data: props.room };
        this.onClick = this.onClick.bind(this);
    }
    onClick(e) {
        this.props.onOrder(this.state.data);
    }

    render() {
        return <div>
            <p><b>{this.state.data.description}</b></p>
            <p>Цена {this.state.data.price}</p>
            <p><button onClick={this.onClick}>Заказать</button></p>
        </div>;
    }
}

class RoomForm extends React.Component {

    constructor(props) {
        super(props);
        this.state = { description: "", price: 0 };

        this.onSubmit = this.onSubmit.bind(this);
        this.onDescriptionChange = this.onDescriptionChange.bind(this);
        this.onPriceChange = this.onPriceChange.bind(this);
    }
    onDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    onPriceChange(e) {
        this.setState({ price: e.target.value });
    }
    onSubmit(e) {
        e.preventDefault();
        var roomDescription = this.state.description.trim();
        var roomPrice = this.state.price;
        if (!roomDescription || roomPrice <= 0) {
            return;
        }
        this.props.onRoomSubmit({ description: roomDescription, price: roomPrice });
        this.setState({ description: "", price: 0 });
    }
    render() {
        return (
            <form onSubmit={this.onSubmit}>
                <p>
                    <input type="text"
                        placeholder="Описание номера"
                        value={this.state.description}
                        onChange={this.onDescriptionChange} />
                </p>
                <p>
                    <input type="number"
                        placeholder="Цена"
                        value={this.state.price}
                        onChange={this.onPriceChange} />
                </p>
                <input type="submit" value="Сохранить" />
            </form>
        );
    }
}

class Rooms extends React.Component {

    constructor(props) {
        super(props);
        this.state = { rooms: [] };

        this.onAddRoom = this.onAddRoom.bind(this);
        this.onOrderRoom = this.onOrderRoom.bind(this);
    }

    // Список номеров
    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("get", this.props.apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            this.setState({ rooms: data });
        }.bind(this);
        xhr.send();
    }
    componentDidMount() {
        this.loadData();
    }

    // Добавление номера
    onAddRoom(room) {
        if (room) {

            const data = new FormData();
            data.append("description", room.description);
            data.append("price", phone.price);
            var xhr = new XMLHttpRequest();

            xhr.open("post", /api/newroom/, true);
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send(data);
        }
    }

    // Заказ номера
    onOrderRoom(room) {

        if (room) {
            var url = "/api/bookroom/?roomId=" + room.roomid + "&userId=1";

            var xhr = new XMLHttpRequest();
            xhr.open("delete", url, true);
            xhr.setRequestHeader("Content-Type", "application/json");
            xhr.onload = function () {
                if (xhr.status === 200) {
                    this.loadData();
                }
            }.bind(this);
            xhr.send();
        }
    }
    render() {
        var order = this.onOrderRoom;

        return <div>
            <RoomForm onRoomSubmit={this.onAddRoom} />
            <h2>Список номеров</h2>

            {
                this.state.rooms.map(function (room) {

                    return <Room key={room.roomid} room={room} onOrder={order} />
                })
            }

        </div>;
    }
}
ReactDOM.render(
    <Rooms apiUrl="/api/getrooms" />,
    document.getElementById("content")
);