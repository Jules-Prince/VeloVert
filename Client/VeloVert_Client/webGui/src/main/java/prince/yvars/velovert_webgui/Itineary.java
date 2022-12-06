package prince.yvars.velovert_webgui;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class Itineary {

    public String departure;
    public String arrival;

    public Itineary() {
    }

    public Itineary(String depart, String arrivee) {
        this.departure = depart;
        this.arrival = arrivee;
    }
}
