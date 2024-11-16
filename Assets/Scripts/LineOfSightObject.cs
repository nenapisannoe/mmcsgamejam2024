using UnityEngine;

public abstract class LineOfSightObject : MonoBehaviour {

    protected bool IsPointInLineOfSight(Vector3 target, bool ignoreDistance = false) {
        var origin = GetOrigin();
        var isInCone = IsPointInsideCone(target, ignoreDistance);
        var isInLineOfSight = false;
        if (isInCone) {
            var direction = target - origin;
            var layerMask = LayerMask.GetMask("Default", "Character");
            var isHit = Physics.Raycast(origin, direction, out RaycastHit hit, Mathf.Infinity, layerMask);
            if (isHit) {
                isInLineOfSight = hit.collider.gameObject.layer == 14;
                Debug.DrawRay(origin, direction * hit.distance, isInLineOfSight ? Color.red : Color.white);
            }
        }

        return isInLineOfSight;
    }

    /// <summary>
    /// ChatGPT saves the day
    ///
    /// Для определения, находится ли точка в трехмерном пространстве внутри конуса, можно использовать следующий
    /// алгоритм:
    /// 1. Проверьте, находится ли точка внутри длины конуса. Это делается путем вычисления проекции вектора от
    /// вершины конуса до точки на вектор направления конуса. Если проекция выходит за пределы длины, точка не в конусе.
    /// 2. Проверьте, находится ли точка внутри угла конуса. Для этого вычислите угол между вектором направления конуса
    /// и вектором от вершины до точки. Если угол больше заданного угла образующей, точка вне конуса.
    /// </summary>
    private bool IsPointInsideCone(Vector3 point, bool ignoreDistance) {
        var origin = GetOrigin();
        var direction = GetDirection();
        var length = ignoreDistance ? 100f : GetLength();
        var angle = Mathf.Deg2Rad * GetAngle();
        Vector3 apexToPoint = point - origin;
        float projectionLength = Vector3.Dot(apexToPoint, direction);
        if (projectionLength < 0 || projectionLength > length) {
            return false;
        }
        var distanceToAxis = (apexToPoint - direction * projectionLength).magnitude;
        float maxRadiusAtProjection = projectionLength * Mathf.Tan(angle);
        return distanceToAxis <= maxRadiusAtProjection;
    }

    protected abstract Vector3 GetOrigin();
    protected abstract Vector3 GetDirection();
    protected abstract float GetLength();
    protected abstract float GetAngle();
    protected abstract Color GetColor();

    protected virtual void OnDrawGizmos() {
        Gizmos.color = GetColor();
        var origin = GetOrigin();
        var direction = GetDirection();
        var length = GetLength();
        var angle = GetAngle();
        Gizmos.DrawLine(origin, origin + direction * length);
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        var angleDirection = rotation * direction;
        Gizmos.DrawLine(origin, origin + angleDirection * length);
        rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        angleDirection = rotation * direction;
        Gizmos.DrawLine(origin, origin + angleDirection * length);
    }
        
}